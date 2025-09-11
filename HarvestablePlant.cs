using System;

namespace NeonQOL
{
    internal class HarvestablePlant
    {
        public int TileTypeImmature;
        public int TileTypeBlooming;
        public int ItemTypePlant;
        public int[] ItemTypePlantList;
        public int ItemTypeSeed;
        public int[] ItemTypeSeedList;
        public int TileStyleImmature;
        public int[] TileStyleImmatureList;
        public int TileStyleBlooming;
        public int[] TileStyleBloomingList;
        public Func<bool> Cond;

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = 0;
            TileStyleBlooming = 0;
            Cond = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = 0;
            Cond = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            Cond = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int itemTypePlant, int itemTypeSeed, int tileStyleImmature, int tileStyleBlooming, Func<bool> cond)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlant = itemTypePlant;
            ItemTypeSeed = itemTypeSeed;
            TileStyleImmature = tileStyleImmature;
            TileStyleBlooming = tileStyleBlooming;
            Cond = cond;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int[] itemTypePlant, int[] itemTypeSeed, int[] tileStyleImmature, int[] tileStyleBlooming)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            Cond = () => true;
        }

        public HarvestablePlant(int tileTypeImmature, int tileTypeBlooming, int[] itemTypePlant, int[] itemTypeSeed, int[] tileStyleImmature, int[] tileStyleBlooming, Func<bool> cond)
        {
            TileTypeImmature = tileTypeImmature;
            TileTypeBlooming = tileTypeBlooming;
            ItemTypePlantList = itemTypePlant;
            ItemTypeSeedList = itemTypeSeed;
            TileStyleImmatureList = tileStyleImmature;
            TileStyleBloomingList = tileStyleBlooming;
            Cond = cond;
        }
    }
}