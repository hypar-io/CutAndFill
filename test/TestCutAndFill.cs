using System;
using System.Collections.Generic;
using Elements;
using Elements.Geometry;
using Elements.Serialization.glTF;
using Xunit;

namespace CutAndFill.Tests
{
    public class CutAndFillTests
    {
        [Fact]
        public void CutAndFillTest()
        {
            var models = new Dictionary<string, Elements.Model>();
            var cells = 128;
            var elevations = new double[(int)Math.Pow(cells, 2)];
            var y = 0;
            var z = 10.0;
            for (var x = 0; x < Math.Pow(cells, 2); x++)
            {
                if (x > cells - 1)
                {
                    y++;
                }
                elevations[x] = z + 2 * Math.Sin(y / cells);
            }

            var topo = new Topography(Vector3.Origin, 500, elevations);
            var model = new Model();
            model.AddElement(topo);
            models["location"] = model;

            var site = Polygon.Rectangle(Vector3.Origin, new Vector3(100, 100));

            var t = new Transform();
            t.Move(20, 20);
            var inputs = new CutAndFillInputs(site, new[] { (Polygon)Polygon.L(20, 10, 5).Transformed(t) }, 10, 45.0, null, null, null, null, null, null);
            var result = CutAndFill.Execute(models, inputs);
            result.Model.AddElement(new ModelCurve(site));
            result.Model.ToGlTF("model.glb");
        }
    }
}