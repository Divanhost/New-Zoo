using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace zoo
{
    // Базовый класс животного
    public class Creature
    {

        // состояние здоровья 
        public State state;
        // Имя
        string name;
        // наблюдатели
        List<Observer> listeners = new List<Observer>();
        //вид
        public string kind;
        // отсчет времени
        public static int livingHours;
        // Голова
        Head head;
        //Туловище
        Body body;
        // Задняя часть
        Tail tail;
        // Множители для расчетов
        public double multiplier;
        public double illMultiplier;
        public Creature()
        {
            state = new Healthy(this);
            multiplier = 1;
            illMultiplier = 1;
            livingHours = 6;
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
        // Проверка здоровья животного
        public void FullCheck(float t,bool d)
        {
            CheckHungry();
            CheckAviary(d);
            CheckTemperature(t);
            Notify();
        }
       
       
        // Проверка соответствия температуры
        public void CheckTemperature(float aviaryTemp)
        {
            if (aviaryTemp - GetTemperature() > 5f || aviaryTemp - GetTemperature() < -5f)
            {
                state.AddProblem("Temperature");
            }
            else
            {
                state.RemoveProblem("Temperature");
            }
        }
        // Проверка загрязненности вольера
        public void CheckAviary(bool dirty)
        {
            if (dirty && new Random().Next(5)>3)
            {
                state.AddProblem("Environment");
            }
            else state.RemoveProblem("Environment");
        }
        // Оголодание 
        public void CheckHungry()
        {
            if (livingHours == 7 || livingHours == 13 || livingHours == 19 || !state.CheckFields()[0])
             {
                 state.AddProblem("Food");
                 Notify();
             }
        }
        // Обновление состояния животного
        public void UpdateHealth()
        {
            Random rng = new Random();
            if (this.state is Healthy)
            {
                bool[] mass = this.state.CheckFields();
                if (mass[0])
                {
                    this.multiplier += rng.NextDouble() / 2;
                }
                if (mass[1])
                {
                    this.multiplier += rng.NextDouble();
                }
                if (mass[2])
                {
                    this.multiplier += rng.NextDouble() / 3;
                }
                double result = rng.Next(3) * this.multiplier;
                if (result > 10f) this.state.UpdateState(true);
            }
            if (this.state is Ill)
            {
                bool[] mass = this.state.CheckFields();
                if (mass[0])
                {
                    this.illMultiplier += rng.NextDouble() / 2;
                }
                if (mass[1])
                {
                    this.illMultiplier += rng.NextDouble();
                }
                if (mass[2])
                {
                    this.illMultiplier += rng.NextDouble() / 3 * 2;
                }
                double result = rng.Next(3) * this.illMultiplier;
                if (result > 10f) this.state.UpdateState(true);
            }
            Notify();
        }
        public void SetHead(Head _head)
        {
            head = _head;
        }
        public void SetBody(Body _body)
        {
            body = _body;
        }
        public void SetTail(Tail _tail)
        {
            tail = _tail;
        }
        public void SetName(string _name)
        {
            name = _name;
        }
        public string GetHead()
        {
            return head.namePart;
        }
        public string GetBody()
        {
            return body.namePart;
        }
        public string GetTail()
        {
            return tail.namePart;
        }
        public string GetName()
        {
            return name;
        }
        public string GetFood()
        {
            return head.specFood;
        }
        public string GetEnvironment()
        {
            return body.specEnvironment;
        }
        public float GetTemperature()
        {
            return tail.specTemperature;
        }
        // создание Случайной комбинации из различных существ
        public static List<string> FindRandomCombination(string addition)
        {
            string path = Application.StartupPath;
            path += '\\' + addition;
            List<string> heads = new List<string>();
            List<string> bodies = new List<string>();
            List<string> tails = new List<string>();
            List<string> food = new List<string>();
            List<string> environment = new List<string>();
            List<string> temperature = new List<string>();
            string[] variants = File.ReadAllLines(path, Encoding.Default);
            foreach (string item in variants)
            {
                string[] tmp = item.Split();
                heads.Add(tmp[0]);
                bodies.Add(tmp[1]);
                tails.Add(tmp[2]);
                food.Add(tmp[3]);
                environment.Add(tmp[4]);
                temperature.Add(tmp[5]);
            }
            Random rnd = new Random();
            List<string> result = new List<string>();
            int hd = rnd.Next(heads.Count);
            int bd = rnd.Next(bodies.Count);
            int tl = rnd.Next(tails.Count);
            result.Add(heads[hd]);
            result.Add(bodies[bd]);
            result.Add(tails[tl]);
            result.Add(food[hd]);
            result.Add(environment[bd]);
            result.Add(temperature[tl]);
            return result;
        }
    }

    // Класс Млекопитащее
    public class Mammal : Creature
    {
        public Mammal()
        {
            kind = "Млекопитающее";
            multiplier = 1.2f;
        }
    }

    // Класс Рыба
    public class Fish : Creature
    {
        public Fish()
        {
            kind = "Рыба";
            illMultiplier = 1.3f;
        }
    }

    // Класс Птица
    public class Bird : Creature
    {
        public Bird()
        {
            kind = "Птица";
            multiplier = 0.9;
        }
    }

    // Класс Рептилия
    public class Reptile : Creature
    {
        public Reptile()
        {
            kind = "Рептилия";
            illMultiplier = 1.7f;
        }
    }
}
