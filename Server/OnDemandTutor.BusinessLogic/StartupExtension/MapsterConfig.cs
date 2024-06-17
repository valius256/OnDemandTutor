using FirebaseAdmin.Auth;
using Mapster;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.BusinessLogic.StartupExtension
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ExportedUserRecord, User>()
                .Map(dest => dest.FireBaseid, src => src.Uid)
                .Map(dest => dest.FirstName, src => src.Email)
                .Map(dest => dest.Password, src => string.Empty)
                ;


        }
    }
}
