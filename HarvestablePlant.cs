using System;
using System.Collections.Generic;

namespace NeonQOL
{
    internal class HarvestablePlant
    {
        public int TileTypeImmature;
        public int TileTypeBlooming;
        public int ItemTypePlant;
        public List<int> ItemTypePlantList;
        public int ItemTypeSeed;
        public List<int> ItemTypeSeedList;
        public int TileStyleImmature;
        public List<int> TileStyleImmatureList;
        public int TileStyleBlooming;
        public List<int> TileStyleBloomingList;
        public Func<bool> CondReplant;
        public Func<bool> CondAutoSelect;
        public Func<bool> CondSmartCursor;

        // type representing a plant in its immature and blooming forms as well as the seed and item it drops, plus optional conditions for interacting with smart regrowth

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = 0;
            TileStyleBlooming = 0;
            CondReplant = () => true;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = 0;
            CondReplant = () => true;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            CondReplant = () => true;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming, Func<bool> condReplant)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming, Func<bool> condReplant, Func<bool> condAutoSelect)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = condAutoSelect;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming, Func<bool> condReplant, Func<bool> condAutoSelect, Func<bool> condSmartCursor)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = condAutoSelect;
            CondSmartCursor = condSmartCursor;
        }


        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, List<int> itemTypePlant, List<int> itemTypeSeed, List<int> tileStyleImmature, List<int> tileStyleBlooming)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            CondReplant = () => true;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, List<int> itemTypePlant, List<int> itemTypeSeed, List<int> tileStyleImmature, List<int> tileStyleBlooming, Func<bool> condReplant)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = () => true;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, List<int> itemTypePlant, List<int> itemTypeSeed, List<int> tileStyleImmature, List<int> tileStyleBlooming, Func<bool> condReplant, Func<bool> condAutoSelect)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = condAutoSelect;
            CondSmartCursor = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, List<int> itemTypePlant, List<int> itemTypeSeed, List<int> tileStyleImmature, List<int> tileStyleBlooming, Func<bool> condReplant, Func<bool> condAutoSelect, Func<bool> condSmartCursor)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            CondReplant = condReplant;
            CondAutoSelect = condAutoSelect;
            CondSmartCursor = condSmartCursor;
        }
    }
}