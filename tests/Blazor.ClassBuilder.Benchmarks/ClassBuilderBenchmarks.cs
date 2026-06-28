using BenchmarkDotNet.Attributes;

namespace Blazor.ClassBuilder.Benchmarks
{
    /// <summary>
    /// Models the real SatisFIT chain ListRowOption._listRowClasses: one constant, one AddIfElse,
    /// and five AddIf with all-literal fragments.
    /// </summary>
    [MemoryDiagnoser]
    public class ClassBuilderBenchmarks
    {
        private const string Base = "flex flex-col";
        private const string WidthOptimized = "grow shrink-0 basis-0";
        private const string FlexAuto = "flex-auto";
        private const string SelectedFilled = "bg-gradient-to-t from-gray-100 to-gray-50";
        private const string SelectedNotFilled = "bg-yellow-300";
        private const string NotSelectedDefault = "bg-theme-yellow-light";
        private const string Cursor = "cursor-pointer";
        private const string Border = "border-r border-gray-200";

        private readonly bool _isWidthOptimized = true;
        private readonly bool _isSelected = true;
        private readonly bool _isFilled = false;
        private readonly bool _isDisabled = false;

        [Benchmark]
        public string ListRowClasses()
        {
            return new ClassBuilder()
                .Add(Base)
                .AddIfElse(_isWidthOptimized, WidthOptimized, FlexAuto)
                .AddIf(_isSelected && _isFilled, SelectedFilled)
                .AddIf(_isSelected && !_isFilled, SelectedNotFilled)
                .AddIf(!_isDisabled && !_isFilled && !_isSelected, NotSelectedDefault)
                .AddIf(!_isDisabled, Cursor)
                .AddIf(!_isSelected, Border)
                .Build();
        }
    }
}
