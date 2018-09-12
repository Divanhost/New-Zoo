using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace zoo
{
    public partial class Form1 : Form
    {
        // Температура на улице
        public static float outsideTemperature = 30f;
        // Список ListBox для отображения вольеров
        public List<ListBox> displays = new List<ListBox>();
        // Список вольеров
        public List<Aviary> aviaries = new List<Aviary>();
        // Штатный ветеринар
        public Vet doctor = new Vet();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           // Заполнение массива Lisboxов
            for (int i = 2; i < 22; i++)
            {
                string str = "listBox" + i;
                Panel a = this.Controls["panel1"] as Panel;
                ListBox lb = a.Controls["listBox" + i] as ListBox;
                lb.Items.Clear();
                displays.Add(lb);
            }
            listBox1.Items.Add("Creation Started");
            
            // Создание живонтых в зоопарке
            for (int i = 0; i < displays.Count; i++)
            {
                int q = new Random().Next(4);
                Builder builder = new MammalCreatureBuilder();
                // Начало создания животных
                if(q==0)  builder = new MammalCreatureBuilder(); //Билдер
                if (q==1) builder = new BirdCreatureBuilder(); //Билдер
                if (q==2) builder = new FishCreatureBuilder(); //Билдер
                if (q==3) builder = new ReptileCreatureBuilder(); //Билдер

                Director director = new Director(builder); // Директор
                director.ConstructRandomCreature();
                Creature crt =builder.GetCreature();
                // Добавление наблюдателей
                crt.Attach(new FoodObserver(crt,new Worker()));
                crt.Attach(new HealthObserver(crt, doctor));
                Aviary a = new Aviary(crt, displays[i],i);
                aviaries.Add(a);
                a.GetInfo();
                // Добавление наблюдателей
                a.Attach(new UpdateObserver(a));
                a.Attach(new ControlTemperatureSystemObserver(a, new Worker()));
                a.Attach(new MessObserver(a, new Worker()));
            }

            listBox1.Items.Add("Creation Successful");
            // Начало отсчета времени
            CurrentTime.Start();
            timer1.Start();
        }
        // Кнопка перезагрузки
        private void button1_Click(object sender, EventArgs e)
        {
            displays.Clear();
            listBox1.Items.Clear();
            foreach (ListBox item in displays)
            {
                item.Items.Clear();
            }
            Form1_Load(this, e);
            label3.Text = "6";
            label4.Text = "0";
        }


        // Таймер для всех автоматических(или случайных) действий
        private void timer1_Tick_1(object sender, EventArgs e)
        {   // Изменение Температуры
            Random rnd = new Random();
            int a = rnd.Next(2);
            bool bl = Convert.ToBoolean(a);
            if (bl) outsideTemperature += 0.1f;
            else outsideTemperature -= 0.1f;
            label1.Text = outsideTemperature.ToString();

            //Возможные события каждого Вольера
            foreach (Aviary item in aviaries)
            {
                if (new Random().Next(5) > 3)
                {
                    // Возможно вольер загрязнится
                    item.state.AddProblem("Dirty");
                }
                item.state.UpdateState(true);
                // Вольер пытается нормализовать свою температуру
                item.TryToNormaliseTemperature();
                // проверка здоровья животных и изменениие их состояния
                item.GetCreature().FullCheck(item.aviaryTemp, item.state.CheckFields()[0]);
                item.GetCreature().UpdateHealth();
            }
            // Ветеринар лечит всех собраных животных
            doctor.TryToCureAnimals();
            
        }

        // Таймер для демонстрации текущего времени
        private void CurrentTime_Tick(object sender, EventArgs e)
        {

            Label hours = this.Controls["label3"] as Label;
            Label minutes = this.Controls["label4"] as Label;
            int h = Convert.ToInt32(hours.Text);
            int m = Convert.ToInt32(minutes.Text);
            m++;
            if (m == 60)
            {
                h++;
                m = 0;
            }
            if (h == 24)
            {
                h = 0;
                m = 0;
            }
           
            hours.Text = h.ToString("D2");
            minutes.Text = m.ToString("D2");
            Worker.workingHours = h;
            Creature.livingHours = h;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            CurrentTime.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            CurrentTime.Start();
        }
    }
 
}
