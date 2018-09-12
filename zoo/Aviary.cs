using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoo
{
    // Класс Вольера
    public class Aviary
    {
        // Номер вольера
        int name;
        // список наблюдателей
        List<Observer> listeners = new List<Observer>();
        public string typeOfAviary;
        // температура внутри вольера
        public float aviaryTemp;
        // Состояние вольера
        public State state;
        // животное в вольере
        private Creature creature;
        // ListBox для отображения
        private ListBox display;
        public Aviary(Creature crt, ListBox lst, int _name)
        {
            state = new PerfectState(this);
            creature = crt;
            aviaryTemp = Form1.outsideTemperature;
            display = lst;
            name = _name;
            if (creature is Fish) typeOfAviary = "Аквариум";
            if (creature is Mammal) typeOfAviary = "Открытый вольер";
            if (creature is Reptile) typeOfAviary = "Закрытый вольер";
            if (creature is Bird) typeOfAviary = "Клетка";

        }
        public Creature GetCreature()
        {
            return creature;
        }

        public int GetName()
        {
            return name;
        }
        // Вывод основной информации о вольере
        public void GetInfo()
        {
            display.Items.Clear();
            display.Items.Add("Name: " + creature.GetName());
            display.Items.Add("Kind: " + creature.kind);
            display.Items.Add("Environment: " + creature.GetEnvironment());
            display.Items.Add("Feeded: " + creature.state.CheckFields()[0]);
            display.Items.Add("Needed temp: " + creature.GetTemperature());
            display.Items.Add("AViary temp: " + Math.Round(aviaryTemp, 1));
            display.Items.Add("Climate sys: " +state.CheckFields()[1]);
            display.Items.Add("Cleanliness: " + state.CheckFields()[0]);
            display.Items.Add(creature.state);
            display.Items.Add(typeOfAviary);
        }
        // Добавление наблюдателя
        public void Attach(Observer obs)
        {
            listeners.Add(obs);
        }
        // обновление наблюдателей
        public void Notify()
        {
            for (int i = 0; i < listeners.Count; i++)
                listeners[i].update();
        }
        // работа системы контроля температуры
        public void TryToNormaliseTemperature()
        {
            if (state.CheckFields()[1])
            {
                Random rng = new Random();
                int a = rng.Next(100);
                if (a > 20)
                {
                    if (aviaryTemp > creature.GetTemperature())
                    {
                        aviaryTemp -= 0.2f;
                    }
                    else if (aviaryTemp < creature.GetTemperature())
                    {
                        aviaryTemp += 0.2f;
                    }

                }
                else  // В процессе работы система может сломаться
                {
                    state.AddProblem("Temperature");
                    float b = Convert.ToSingle(rng.Next(7));
                    if (rng.Next(2) == 0)
                    {
                        aviaryTemp += b;
                    }
                    else aviaryTemp -= b;

                }
            }
            Notify();
        }
    
    }
}
