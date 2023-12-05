namespace SNG.Save
{
    [System.Serializable]
    public class PlayerData
    {
        // put relevant fields here and set their values in the constructor
        // money, level etc
        public long Money;
        public long Experience;
        public long PlayerLevel;
        public long NecessaryExperience;
        public float FillAmount;

        public PlayerData()
        {
            Money = 1000;
            Experience = 0;
            PlayerLevel = 1;
            NecessaryExperience = 40;
            FillAmount = 0f;

        }

        public void ChangeMoney(int moneyAmount)
        {
            Money += moneyAmount;
        }

        public void ChangeExperience(int expAmount)
        {
            Experience += expAmount;
        }

        public void ChangeNecessaryExperience(long lvl)
        {
            NecessaryExperience = 20 * (lvl+1) * (lvl+1) - 20 * (lvl+1);
        }
    }
}