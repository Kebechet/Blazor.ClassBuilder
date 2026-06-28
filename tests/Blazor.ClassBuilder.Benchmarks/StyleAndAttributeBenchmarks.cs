using System.Collections.Generic;
using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Blazor.ClassBuilder.Benchmarks
{
    /// <summary>
    /// Models the real SatisFIT AdaptiveVirtualize styles: a container style that is static across
    /// renders and a per-item style whose translateY changes every render, the way a scrolling list
    /// produces a new value on each frame.
    /// </summary>
    [MemoryDiagnoser]
    public class StyleBuilderBenchmarks
    {
        private int _offset;

        [Benchmark]
        public string ContainerStyle()
        {
            return new StyleBuilder()
                .Add("position", "relative")
                .Add("width", "100%")
                .Add("height", "1200px")
                .Build();
        }

        [Benchmark]
        public string ItemStyle()
        {
            var translate = $"translateY({NextOffset().ToString(CultureInfo.InvariantCulture)}px)";

            return new StyleBuilder()
                .Add("position", "absolute")
                .Add("top", "0")
                .Add("left", "0")
                .Add("width", "100%")
                .Add("transform", translate)
                .Build();
        }

        private int NextOffset()
        {
            _offset += 40;

            return _offset;
        }
    }

    /// <summary>
    /// Models the real SatisFIT chain LazyImage._imageAttributes: two unconditional attributes and
    /// three conditional ones.
    /// </summary>
    [MemoryDiagnoser]
    public class AttributeBuilderBenchmarks
    {
        private const string Src = "https://cdn.satis.fit/exercise/123.webp?v=42";
        private const string Alt = "Barbell back squat";

        private readonly bool _isLazy = true;
        private readonly bool _hasSyncDecoding = true;
        private readonly bool _shouldUseCors = false;

        [Benchmark]
        public Dictionary<string, object> ImageAttributes()
        {
            return new AttributeBuilder()
                .Add("src", Src)
                .Add("alt", Alt)
                .AddIf(_isLazy, "loading", "lazy")
                .AddIf(_hasSyncDecoding, "decoding", "sync")
                .AddIf(_shouldUseCors, "crossorigin", "anonymous")
                .Build();
        }
    }
}
