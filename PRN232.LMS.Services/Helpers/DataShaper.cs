using System.Dynamic;
using System.Reflection;

namespace PRN232.LMS.Services.Helpers;

public static class DataShaper
{
    public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> entities, string? fields)
    {
        var propertyInfos = GetPropertyInfos<T>(fields);

        foreach (var entity in entities)
        {
            yield return FetchDataForEntity(entity, propertyInfos);
        }
    }

    private static IEnumerable<PropertyInfo> GetPropertyInfos<T>(string? fields)
    {
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (string.IsNullOrWhiteSpace(fields))
        {
            return propertyInfos;
        }

        var requiredFields = fields.Split(',')
            .Select(field => field.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return propertyInfos.Where(property => requiredFields.Contains(property.Name));
    }

    private static ExpandoObject FetchDataForEntity<T>(T entity, IEnumerable<PropertyInfo> propertyInfos)
    {
        var shapedObject = new ExpandoObject();
        var dict = (IDictionary<string, object?>)shapedObject;

        foreach (var propertyInfo in propertyInfos)
        {
            dict[propertyInfo.Name] = propertyInfo.GetValue(entity);
        }

        return shapedObject;
    }
}
