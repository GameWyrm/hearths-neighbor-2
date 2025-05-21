using System.Collections.Generic;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class KeyPlacer
    {
        public static Dictionary<string, Vector3> keyLocations = new Dictionary<string, Vector3>()
        {
            { "MAGISTARIUM_Body", new Vector3 (-84.8f, -11.2f, -42.6f) },
            { "StarshipCommunity_Body", new Vector3 (0, 28, 0.4f) },
            { "TheBoiledEgg_Body", new Vector3 (8.5f, 302, -8.5f) },
            { "EggStar_Body", new Vector3 (1449.391f, -260.9305f, 54.7762f) },
            { "ALTTH_Body", new Vector3 (-80.8618f, 130.2982f, -174.0401f) },
            { "Gravelrock_Body", new Vector3 (26, -84.5f, 50) },
            { "FracturedHarmony_Body", new Vector3 (-108.9126f, -118.9276f, 120.3744f) },
            { "FinisPlateau_Body", new Vector3 (116.8198f, 17.4145f, 395.4864f) },
            { "ModJamHub_Body", new Vector3 (8.4109f, 182.8952f, -1.7018f) },
            { "EchoHike_Body", new Vector3 (93, -62, 150) },
            { "Axiom_Body", new Vector3 (37.6543f, -134.4332f, -21f) },
            { "ProjectionSimulation_Body", new Vector3 (-306, -41.5f, -360) }

        };

        public static void PlaceKeys(GameObject keyObject)
        {
            foreach (var body in keyLocations.Keys)
            {
                GameObject targetBody = GameObject.Find(body);
                if (targetBody == null)
                {
                    HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Could not locate planet {targetBody}. The third contest is incomplete...");
                }
                else
                {
                    HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Creating key on {body}");
                    GameObject key = GameObject.Instantiate(keyObject, targetBody.transform);
                    key.transform.localPosition = keyLocations[body];
                }
            }
        }
    }
}
