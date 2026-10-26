using BackEnd.Entities;
using Microsoft.OpenApi.Models;

namespace BackEnd.Entities.Level
{
    public class Level : BaseEntity
    {
        public int XPToReach;
        public int LevelNumber;
    }
}
