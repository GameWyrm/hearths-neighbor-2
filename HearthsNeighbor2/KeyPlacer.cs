using System.Collections.Generic;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class KeyPlacer
    {
        public static Dictionary<string, Vector3> keyLocations = new Dictionary<string, Vector3>()
        {
            { "MAGISTARIUM", new Vector3 (-84.8f, -11.2f, -42.6f) },
            { "StarshipCommunity", new Vector3 (0, 28, 0.4f) },
            { "TheBoiledEgg", new Vector3 (8.5f, 302, -8.5f) },
            { "EggStar", new Vector3 (872.597f, -83.2182f, 29f) },
            { "ALTTH", new Vector3 (-80.8618f, 130.2982f, -174.0401f) },
            { "Gravelrock", new Vector3 (26, -84.5f, 50) },
            { "FracturedHarmony", new Vector3 (-108.9126f, -118.9276f, 120.3744f) },
            { "FinisPlateau", new Vector3 (116.8198f, 17.4145f, 395.4864f) },
            { "ModJamHub", new Vector3 (8.4109f, 182.8952f, -1.7018f) },
            { "EchoHike", new Vector3 (93, -62, 150) },
            { "Axiom", new Vector3 (37.6543f, -134.4332f, -21f) },
            { "ProjectionSimulation", new Vector3 (-306, -41.5f, -360) }

        };

        public static void PlaceKeys(GameObject keyObject)
        {
            foreach (var body in keyLocations.Keys)
            {
                if (PlayerData.GetPersistentCondition("HN2_KEY_" + body)) continue;
                GameObject targetBody = GameObject.Find(body + "_Body");
                if (targetBody == null)
                {
                    HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Could not locate planet {targetBody}. The third contest is incomplete...");
                }
                else
                {
                    HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Creating key on {body}");
                    GameObject key = GameObject.Instantiate(keyObject, targetBody.transform);
                    key.transform.localPosition = keyLocations[body];
                    key.GetComponent<MagiKeyInteraction>().conditionKey = "HN2_KEY_" + body;
                    key.GetComponent<MagiKeyInteraction>().shipLogEntry = "HN20_" + body;
                }
            }
        }
    }
}
