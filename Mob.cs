using System;

namespace ActionGo.Scenes
{
    public class Mob
    {
        public string Name { get; private set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Mob(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }
    }
}
