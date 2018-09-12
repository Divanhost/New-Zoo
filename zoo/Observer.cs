using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoo
{
    //Наблюдатель
    public abstract class Observer
    {
        protected Aviary aviary; // наблюдаемый вольер
        protected Creature creature; // наблюдаемое животное
        protected Worker worker; // работник
        protected Vet doctor; // ветеринар
        public Observer()
        {
        }
        public abstract void update();
    }
    // для обновления информации
    public class UpdateObserver : Observer
    {
        public UpdateObserver(Aviary _host)
        {
            aviary = _host;
            aviary.Attach(this);
        }
        public override void update()
        {
            aviary.GetInfo();
        }
    }
    // для Контроля здоровья животного
    public class HealthObserver : Observer
    {
        public HealthObserver(Creature _creatre, Vet _doctor)
        {
            creature = _creatre;
            doctor = _doctor;
            creature.Attach(this);
        }
        public override void update()
        {
            if(creature.state is Ill)
            {
                doctor.AddAnimalInCuringQueue(creature);
            }

        }
    }
    // для контроля температуры в вольере
    public class ControlTemperatureSystemObserver : Observer
    {
        
        public ControlTemperatureSystemObserver(Aviary _host, Worker a)
        {
            aviary = _host;
            aviary.Attach(this);
            worker = a;
        }
        public override void update()
        {
            if (!aviary.state.CheckFields()[1])
            worker.TryToRepair(aviary);
        }
    }
    // для контроля еды
    public class FoodObserver : Observer
    {
        public FoodObserver(Creature _creature,Worker a)
        {
            creature = _creature;
            worker = a;
        }
        public override void update()
        {
            worker.TryToFeedAnimal(creature);
        }
    }
    // для контроля порядка в вольере
    public class MessObserver : Observer
    {
        public MessObserver(Aviary _host, Worker a) 
        {
            aviary = _host;
            aviary.Attach(this);
            worker = a;
        }
        public override void update()
        {
           worker.TryToCleanAviary(aviary);
        }
    }
    
}
