namespace Clear.Data
{
    public class GameData
    {
        public int level;
        public int health;
        public int points;

        public GameData(int health)
        {
            level = 1;
            this.health = health;
            points = 0;
        }
    }
}