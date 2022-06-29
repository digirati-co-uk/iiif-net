using System;
using System.Text;

namespace IIIF.ImageApi
{
    /// <summary>
    /// Represents the {size} parameter of a IIIF image request.
    /// </summary>
    /// <remarks>see https://iiif.io/api/image/3.0/#42-size </remarks>
    public class SizeParameter
    {
        public int? Width { get; set; }
        
        public int? Height { get; set; }
        
        public bool Max { get; set; }
        
        public bool Upscaled { get; set; }
        
        public bool Confined { get; set; }
        
        public float? PercentScale { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Upscaled)
            {
                sb.Append('^');
            }
            if (Max)
            {
                sb.Append("max");
                return sb.ToString();
            }
            if (Confined)
            {
                sb.Append('!');
            }
            if (PercentScale > 0)
            {
                sb.Append("pct:" + PercentScale);
                return sb.ToString();
            }
            if (Width > 0)
            {
                sb.Append(Width);
            }
            sb.Append(',');
            if (Height > 0)
            {
                sb.Append(Height);
            }

            return sb.ToString();
        }

        public static SizeParameter Parse(ReadOnlySpan<char> pathPart)
        {
            var size = new SizeParameter();

            if (pathPart[0] == '^')
            {
                size.Upscaled = true;
                pathPart = pathPart[1..];
            }

            if (pathPart.Equals("max".AsSpan(), StringComparison.Ordinal) ||
                pathPart.Equals("full".AsSpan(), StringComparison.Ordinal))
            {
                size.Max = true;
                return size;
            }

            if (pathPart[0] == '!')
            {
                size.Confined = true;
                pathPart = pathPart[1..];
            }

            if (pathPart[0] == 'p')
            {
                size.PercentScale = float.Parse(pathPart[4..]);
                return size;
            }

            var workingBuilder = new StringBuilder(); 
            foreach (var t in pathPart)
            {
                if (t == ',')
                {
                    if (workingBuilder.Length > 0)
                    {
                        size.Width = int.Parse(workingBuilder.ToString());
                    }
                    workingBuilder = new StringBuilder();

                    continue;
                }

                workingBuilder.Append(t);
            }

            if (workingBuilder.Length > 0)
            {
                size.Height = int.Parse(workingBuilder.ToString());
            }
            
            return size;
        }
    }
}
