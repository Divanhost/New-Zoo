using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoo
{
    // Класс работника зоопарка
    public class Worker
    {
        public static int workingHours;
        // попытка починить системы обогрева вольера
        public void TryToRepair(Aviary host)
        {
            if (!host.state.CheckFields()[1])
            {
                Random rng = new Random();
                int a = rng.Next(5);
                if (a > 3)
                {
                    host.state.RemoveProblem("Temperature");
                    host.state.UpdateState(true);
                }
            }
        }
        // попытка покормить животное
        public void TryToFeedAnimal(Creature animal)
        {
            if (!animal.state.CheckFields()[0]  || (workingHours == 7 || workingHours == 13 || workingHours == 19))
            {
                Random rng = new Random();
                int a = rng.Next(5);
                if (a > 2)
                {
                    animal.state.RemoveProblem("Food");
                }
            }
        }
        // попытка почистить вольер
        public void TryToCleanAviary(Aviary host)
        {
            if (!host.state.CheckFields()[0])
            {
                Random rng = new Random();
                int a = rng.Next(5);
                if (a > 3)
                {
                    host.state.RemoveProblem("Dirty");
                    host.state.UpdateState(true);
                }
            }
        }
    }
    // Класс ветеринара
    public class Vet
    {
        // животные, требующие лечения
        List<Creature> animals;
        public Vet()
        {
            animals = new List<Creature>();
        }
        // добавление животных в список на лечение
        public void AddAnimalInCuringQueue(Creature animal)
        {
            animals.Add(animal);
        }
        // попытка вылечить живонтых
        public void TryToCureAnimals()
        {
            foreach (Creature crt in animals)
            {
                if (!(crt.state is Dead) && crt.state is Ill)
                {
                    Random rng = new Random();
                    int a = rng.Next(100);
                    if (a > 10)
                    {
                        crt.state.RemoveProblem("Temperature");
                        crt.state.RemoveProblem("Food");
                        crt.state.RemoveProblem("Environment");
                        crt.state.UpdateState(false);
                        crt.multiplier = 1f;
                        crt.illMultiplier = 1f;
                    }
                    if (a == 0) crt.state.UpdateState(true);
                }
            }
        }
       
    }
}
