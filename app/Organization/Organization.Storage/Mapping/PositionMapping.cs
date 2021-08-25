using AutoMapper;
using Organization.Share.Model;
using Organization.Storage.Entity;

namespace Organization.Storage.Mapping
{
    public class PositionMapping : Profile
    {
        public PositionMapping()
        {
            CreateMap<Position, PositionModel>();
            CreateMap<PositionModel, Position>();
        }
    }
}
