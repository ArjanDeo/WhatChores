using Models.BattleNet.Public;
using Models.RaiderIO.MythicPlus;

namespace Models.WhatChores
{
    public class OverviewModel
    {
        public WoWTokenPriceModel WoWTokenData { get; set; }
        public RaiderIOMythicPlusAffixesModel AffixData { get; set; }
    }
}
