using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace Questionnaire.SharedKernel.Services;

public interface IMapper<in TSource, out TDestination>
{
    TDestination Map(TSource source);
}

public interface IMappingService
{
    TDestination Map<TSource, TDestination>(TSource source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}

public class MappingService(IServiceProvider serviceProvider) : IMappingService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private static readonly ConcurrentDictionary<(Type Source, Type Dest), PropertyInfo[]> _sourcePropCache = new();
    private static readonly ConcurrentDictionary<(Type Source, Type Dest), PropertyInfo[]> _destPropCache = new();

    public TDestination Map<TSource, TDestination>(TSource source)
    {

        if (source is null) 
            return default!;

        // 1) Prefer an explicitly registered mapper for this type pair.
        var customMapper = _serviceProvider.GetService<IMapper<TSource, TDestination>>();

        if (customMapper is not null) 
            return customMapper.Map(source);

        // 2) Fall back to reflection-based property copying.
        var destination = Activator.CreateInstance<TDestination>();

        return MapByReflection(source, destination);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source is null) return destination;

        var customMapper = _serviceProvider.GetService<IMapper<TSource, TDestination>>();

        if (customMapper is not null)
        {
            var mapped = customMapper.Map(source);
            return CopyOnto(mapped, destination);
        }

        return MapByReflection(source, destination);
    }

    #region HelperMethods 

    private static TDestination MapByReflection<TSource, TDestination>(TSource source, TDestination destination)
    {
        var key = (typeof(TSource), typeof(TDestination));

        var sourceProps = _sourcePropCache.GetOrAdd(key, _ =>
            typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToArray());

        var destProps = _destPropCache.GetOrAdd(key, _ =>
            typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToArray());

        foreach (var sourceProp in sourceProps)
        {
            var destProp = destProps.FirstOrDefault(p =>
                p.Name == sourceProp.Name &&
                p.PropertyType.IsAssignableFrom(sourceProp.PropertyType));

            if (destProp is null) continue;

            var value = sourceProp.GetValue(source);
            destProp.SetValue(destination, value);
        }

        return destination;
    }

    private static TDestination CopyOnto<TDestination>(TDestination from, TDestination onto)
    {
        var props = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite);

        foreach (var p in props)
            p.SetValue(onto, p.GetValue(from));

        return onto;
    }

    #endregion
}
