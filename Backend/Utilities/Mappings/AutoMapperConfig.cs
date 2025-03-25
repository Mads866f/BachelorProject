using AutoMapper;

namespace Backend.Utilities.Mappings;

public static class AutoMapperConfig
{
    public static MapperConfiguration ConfigureMappings()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ElectionProfile>();
            cfg.AddProfile<ProjectProfile>();
        });
    }
}