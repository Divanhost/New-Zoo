using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace zoo
{
    // Руководство сборкой
    class Director
    {
        Builder builder;
        public Director(Builder builder)
        {
            this.builder = builder;
        }
        // Порядок сборки
        public void ConstructRandomCreature()
        {
            builder.CreateNewCreature();
            builder.BuildHead();
            builder.BuildBody();
            builder.BuildTail();
        }
    }
    abstract class Builder
    {
        // Добавление передней части тела
        public abstract void BuildHead();
        // Добавление Средней части тела
        public abstract void BuildBody();
        // Добавление задней части тела
        public abstract void BuildTail();
        // Создание животного
        public abstract void CreateNewCreature();
        // Возвращает готовое животное
        public abstract Creature GetCreature();
    }
    class MammalCreatureBuilder : Builder
    {
        Creature creature;
        List<string> parts = new List<string>();
        string addition = "млекопитающие.txt";
        public override void CreateNewCreature()
        {
            creature = new Mammal();
            parts.Clear();
            parts = Creature.FindRandomCombination(addition);
            creature.SetName(parts[0] + parts[1] + parts[2]);
            Thread.Sleep(20);
        }
        public override void BuildHead()
        {
            creature.SetHead(new Head(parts[0], parts[3]));
        }
        public override void BuildBody()
        {
            creature.SetBody(new Body(parts[1], parts[4]));
        }
        public override void BuildTail()
        {
            creature.SetTail(new Tail(parts[2], Convert.ToSingle(parts[5])));
        }
        public override Creature GetCreature()
        {
            return creature;
        }
    }
    class FishCreatureBuilder : Builder
    {
        Creature creature;
        List<string> parts = new List<string>();
        string addition = "рыбы.txt";

        public override void CreateNewCreature()
        {
            creature = new Fish();
            parts.Clear();
            parts = Creature.FindRandomCombination(addition);
            creature.SetName(parts[0] + parts[1] + parts[2]);
            Thread.Sleep(20);
        }
        public override void BuildHead()
        {
            creature.SetHead(new Head(parts[0], parts[3]));
        }
        public override void BuildBody()
        {
            creature.SetBody(new Body(parts[1], parts[4]));
        }
        public override void BuildTail()
        {
            creature.SetTail(new Tail(parts[2], Convert.ToSingle(parts[5])));
        }
        public override Creature GetCreature()
        {
            return creature;
        }
    }
    class BirdCreatureBuilder : Builder
    {
        Creature creature;
        List<string> parts = new List<string>();
        string addition = "птицы.txt";

        public override void CreateNewCreature()
        {
            creature = new Bird();
            parts.Clear();
            parts = Creature.FindRandomCombination(addition);
            creature.SetName(parts[0] + parts[1] + parts[2]);
            Thread.Sleep(20);
        }
        public override void BuildHead()
        {
            creature.SetHead(new Head(parts[0], parts[3]));
        }
        public override void BuildBody()
        {
            creature.SetBody(new Body(parts[1], parts[4]));
        }
        public override void BuildTail()
        {
            creature.SetTail(new Tail(parts[2], Convert.ToSingle(parts[5])));
        }
        public override Creature GetCreature()
        {
            return creature;
        }
    }
    class ReptileCreatureBuilder : Builder
    {
        Creature creature;
        List<string> parts = new List<string>();
        string addition = "рептилии.txt";

        public override void CreateNewCreature()
        {
            creature = new Reptile();
            parts.Clear();
            parts = Creature.FindRandomCombination(addition);
            creature.SetName(parts[0] + parts[1] + parts[2]);
            Thread.Sleep(20);
        }

        public override void BuildHead()
        {
            creature.SetHead(new Head(parts[0], parts[3]));
        }
        public override void BuildBody()
        {
            creature.SetBody(new Body(parts[1], parts[4]));
        }
        public override void BuildTail()
        {
            creature.SetTail(new Tail(parts[2], Convert.ToSingle(parts[5])));
        }
        public override Creature GetCreature()
        {
            return creature;
        }
    }
    public class Head
    {
        public string namePart; //часть имени
        public string specFood; // еда Животного
        public Head(string name, string se)
        {
            namePart = name;
            specFood = se;
        }
    }
    public class Body
    {
        public string namePart; // часть имени
        public string specEnvironment; // среда обитания
        public Body(string name, string se)
        {
            namePart = name;
            specEnvironment = se;
        }
    }
    public class Tail
    {
        public string namePart; // часть имени
        public float specTemperature; // Рекомендуемая температура
        public Tail(string name, float se)
        {
            namePart = name;
            specTemperature = se;
        }
    }
}
