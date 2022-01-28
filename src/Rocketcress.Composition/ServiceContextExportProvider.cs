using Rocketcress.Core;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Rocketcress.Composition;

/// <summary>
/// Represents an <see cref="ExportProvider"/> that uses a <see cref="ServiceContext"/> as export store.
/// </summary>
public class ServiceContextExportProvider : ExportProvider
{
    private readonly ServiceContext _serviceContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContextExportProvider"/> class.
    /// </summary>
    public ServiceContextExportProvider()
        : this(ServiceContext.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContextExportProvider"/> class.
    /// </summary>
    /// <param name="serviceContext">The service context to use.</param>
    public ServiceContextExportProvider(ServiceContext serviceContext)
    {
        _serviceContext = Guard.NotNull(serviceContext);

        var container = new CompositionContainer(this);
        serviceContext.AddInstance<CompositionContainer>(container);
    }

    /// <summary>
    /// Gets all the exports that match the constraint defined by the specified definition.
    /// </summary>
    /// <param name="definition">The object that defines the conditions of the <see cref="Export"/> objects to return.</param>
    /// <param name="atomicComposition">The transactional container for the composition.</param>
    /// <returns>A collection that contains all the exports that match the specified condition.</returns>
    protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
    {
        Guard.NotNull(definition);
        var type = Type.GetType(definition.ContractName);
        if (type == null)
        {
            type = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(definition.ContractName)).FirstOrDefault(x => x != null);
            if (type == null)
                throw new Exception($"Type with name '{definition.ContractName}' could not be found.");
        }

        return new[] { new Export(definition.ContractName, () => _serviceContext.GetInstance(type)) };
    }
}
