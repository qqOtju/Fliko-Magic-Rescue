using MyAssets.Scripts.Data.SO;

namespace MyAssets.Scripts.Data
{
    public class DataOfGame
    {
        public DataGold DataGold { get; }
        public DataScore DataScore { get; }
        public DataAbility[] Abilities { get; }
        public DataPlayer DataPlayer { get; }
        
        public DataOfGame(AbilitySO[] abilitiesSO, PlayerSO playerSO)
        {
            Abilities = new DataAbility[abilitiesSO.Length];
            for (var i = 0; i < abilitiesSO.Length; i++)
                Abilities[i] = new DataAbility(abilitiesSO[i]);
            DataPlayer = new DataPlayer(playerSO);
            DataGold = new DataGold();
            DataScore = new DataScore();
        }
    }
}