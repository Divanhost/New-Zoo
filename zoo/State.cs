using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace zoo
{
    // Базовый класс Состояния
    public class State
    {
        protected bool HasProblems;
        // Обновление Состояния
        public virtual void UpdateState(bool A) { }
        // Функции для работы с полями Состояния (установка их в true/false)
        public virtual void AddProblem(string a) { }
        public virtual void RemoveProblem(string a) { }
        // Сбор информации о текущем состоянии
        public virtual bool[] CheckFields() { return new bool[1]; }
    }
    // Классы для состояния вольера
    public class AviaryState : State
    {
        protected Aviary aviary; // Вольер
        public bool IsClear; // Чистота
        public bool IsTemperatureNormaliserWorking; // Функционирование системы контроля температуры
        public AviaryState(Aviary _aviary)
        {
            aviary = _aviary;
        }
        public override bool[] CheckFields()
        {
            bool[] mass = new bool[2];
            mass[0] = IsClear;
            mass[1] = IsTemperatureNormaliserWorking;
            return mass;

        }
        public override void AddProblem(string a)
        {
            if (a == "Dirty")
            {
                IsClear = false;
                HasProblems = true;
                Form1.listBox1.Items.Add("Aviary #" + aviary.GetName() + " was polluted");
            }
            if (a == "Temperature")
            {
                Form1.listBox1.Items.Add("Aviary's #" + aviary.GetName() + " climate system was broken");
                IsTemperatureNormaliserWorking = false;
                HasProblems = true;
            }
        }
        public override void RemoveProblem(string a)
        {
            if (a == "Dirty" && !IsClear)
            {
                IsClear = true;
                Form1.listBox1.Items.Add("Aviary #" + aviary.GetName() + " was cleaned");
                if (IsTemperatureNormaliserWorking)
                {
                    HasProblems = false;
                }
            }
            if (a == "Temperature" && !IsTemperatureNormaliserWorking)
            {
                IsTemperatureNormaliserWorking = true;
                Form1.listBox1.Items.Add("Aviary's #" + aviary.GetName() + " climate system was repaired");
                if (IsClear)
                {
                    HasProblems = false;
                }
            }
        }
    }
    // Состояние вольера без проблем
    public class PerfectState : AviaryState
    {
        public PerfectState(Aviary _aviary):base(_aviary)
        {
            IsClear = true;
            IsTemperatureNormaliserWorking = true;
        }
        // Возможный переход в альтернативное состояние
        public override void UpdateState(bool a)
        {
            if (HasProblems)
            {
                aviary.state = new ProblemState(aviary, IsClear, IsTemperatureNormaliserWorking);
            }
        }
    }
    // Состояние вольера, имеющего неполадки
    public class ProblemState : AviaryState
    {
        public ProblemState(Aviary _aviary,bool _isClear, bool _IsTemperatureNormaliserWorking) : base(_aviary)
        {
            IsClear = _isClear;
            IsTemperatureNormaliserWorking = _IsTemperatureNormaliserWorking;
        }
        // Возможный переход в альтернативное состояние
        public override void UpdateState(bool a)
        {
            if (!HasProblems)
            {
                aviary.state = new PerfectState(aviary);
            }
        }
    }
    // Состояния животного
    public class AnimalState:State
    {
        protected Creature creature; // животное
        public bool IsTemperatureComfortable; // опасность перегрева
        public bool IsFeeded; // сытость
        public bool IsCleanEnvironment; // чистота вольера
        public AnimalState(Creature _creature)
        {
            creature = _creature;
        }

        public override bool[] CheckFields()
        {
            bool[] mass = new bool[3];
            mass[0] = IsFeeded;
            mass[1] = IsTemperatureComfortable;
            mass[2] = IsCleanEnvironment;
            return mass;

        }
        public override void AddProblem(string a)
        {
            if (a == "Temperature")
            {
                IsTemperatureComfortable = false;
                HasProblems = true;
                Form1.listBox1.Items.Add(creature.GetName() + " is feeling uncomfortable");
            }
            if (a == "Food")
            {
                IsFeeded = false;
                HasProblems = true;
                Form1.listBox1.Items.Add(creature.GetName() + " is hungry");
            }
            if (a == "Environment")
            {
                IsCleanEnvironment = false;
                HasProblems = true;
            }

        }
        public override void RemoveProblem(string a)
        {
            if (a == "Temperautre" && !IsTemperatureComfortable)
            {
                IsTemperatureComfortable = true;
                if (IsFeeded && IsCleanEnvironment) HasProblems = false;
                Form1.listBox1.Items.Add(creature.GetName() + " is feeling comfortable");
            }
            if (a == "Food" && !IsFeeded)
            {
                IsFeeded = true;
                Form1.listBox1.Items.Add(creature.GetName() + " was feeded");
                if (IsTemperatureComfortable && IsCleanEnvironment) HasProblems = false;
            }
            if (a == "Environment" && !IsCleanEnvironment)
            {
                IsCleanEnvironment = true;
                if (IsFeeded && IsTemperatureComfortable) HasProblems = false;
            }
        }
    }
    // Состояние, когда животное здорово
    public class Healthy : AnimalState
    {
        public Healthy(Creature _crt) : base(_crt)
        {
            IsTemperatureComfortable = true;
            IsFeeded = true;
            IsCleanEnvironment = true;
        }
        public override void UpdateState(bool readytoIll)
        {
            if (HasProblems)
            {
                Form1.listBox1.Items.Add(creature.GetName() + " got ill");
                creature.state = new Ill(creature, IsTemperatureComfortable, IsFeeded, IsCleanEnvironment);
            }
        }
    }
    // Состояние, когда животное больно
    public class Ill : AnimalState
    {
        public Ill(Creature _crt, bool _IsTemperatureComfortable, bool _IsFeeded,  bool _IsCleanEnvironment) : base(_crt)
        {
            IsTemperatureComfortable = _IsTemperatureComfortable;
            IsFeeded = _IsFeeded;
            IsCleanEnvironment = _IsCleanEnvironment;
        }
        public override void UpdateState(bool readyToDie)
        {
            if (!HasProblems)
            {
                Form1.listBox1.Items.Add(creature.GetName() + " became healthy");
                creature.state = new Healthy(creature);
            }
            if (readyToDie)
            {
                creature.state = new Dead(creature);
                Form1.listBox1.Items.Add(creature.GetName() + " was dead");

            }
        }
    }
    // Состояние, когда животное умерло
    public class Dead : AnimalState
    {
        public Dead(Creature _crt) : base(_crt)
        {
        }
    }
}
