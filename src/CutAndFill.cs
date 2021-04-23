using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CutAndFill
{
    public static class CutAndFill
    {
        /// <summary>
        /// The CutAndFill function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A CutAndFillOutputs instance containing computed results and the model with any new elements.</returns>
        public static CutAndFillOutputs Execute(Dictionary<string, Model> inputModels, CutAndFillInputs input)
        {
            var model = new Model();

            var locationModel = inputModels["location"];
            var topos = locationModel.AllElementsOfType<Topography>();
            if (topos.Count() == 0)
            {
                throw new System.Exception("No topographies were found.");
            }

            var origin = locationModel.AllElementsOfType<Origin>().ElementAt(0);

            var topo = (Topography)topos.ElementAt(0);
            topo.Material = new Material("LightTopo", Colors.Beige);
            if (input.SitePerimeter != null)
            {
                topo.Trim(input.SitePerimeter);
            }

            var elevation = topo.MinElevation - input.Elevation < 200 ? input.Elevation : topo.MinElevation - 200;

            var results = topo.CutAndFill(input.Perimeters, origin.Elevation + input.Elevation, out List<Mesh> _, out List<Mesh> fills, input.BatterAngle);

            // Demonstrate visualization of the fill volumes.
            model.AddElement(BuiltInMaterials.XAxis);
            foreach (var fill in fills)
            {
                var fillElement = new MeshElement(fill, BuiltInMaterials.XAxis, topo.Transform);
                model.AddElement(fillElement, false);
            }

            // Create temporary envelopes
            foreach (var p in input.Perimeters)
            {
                var e = new Envelope(p, input.Elevation, 30, Vector3.ZAxis, 0.0, new Transform(), BuiltInMaterials.Mass, null, false, Guid.NewGuid(), "Envelope");
                model.AddElement(e);
            }

            var normalMaterial = new Material("Normals", Colors.Gray);
            model.AddElement(normalMaterial);

            model.AddElement(topo);

            var excavationPrice = 450.0;
            var excavationCost = $"${results.CutVolume * excavationPrice:0,0}";
            var dirtMovement = 30.0;
            var siteBalancingCost = $"${dirtMovement * Math.Max(results.CutVolume, results.CutVolume - results.FillVolume):0,0}";
            var output = new CutAndFillOutputs(results.CutVolume, results.FillVolume, excavationCost, siteBalancingCost)
            {
                Model = model
            };
            Console.WriteLine($"Topo Id: {topo.Id}");
            output.Model.AddElement(topo);
            return output;
        }
    }
}