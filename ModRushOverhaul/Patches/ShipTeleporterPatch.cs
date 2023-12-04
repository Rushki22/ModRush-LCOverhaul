using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class ShipTeleporterPatch {

        // Important Assets
        private static AudioClip teleporterBeamUpSFX;
        private static AudioSource shipTeleporterAudio;

        // Override method and cancel original code
        [HarmonyPatch("TeleportPlayerOutWithInverseTeleporter")]
        [HarmonyPrefix] // Do it before original code
        private static bool doNotLoseItemsInverse(ref int playerObj, ref Vector3 teleportPos) {

            if (StartOfRound.Instance.allPlayerScripts[playerObj].isPlayerDead) {

                StartCoroutine(teleportBodyOut(playerObj, teleportPos));
                return false; // Cancel original code
            }

            PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerObj];
            SetPlayerTeleporterId(playerControllerB, -1);
            // Removed DropAllHeldItems() method

            if ((bool)Object.FindObjectOfType<AudioReverbPresets>())
                Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2].ChangeAudioReverbForPlayer(playerControllerB);

            playerControllerB.isInElevator = false;
            playerControllerB.isInHangarShipRoom = false;
            playerControllerB.isInsideFactory = true;
            playerControllerB.averageVelocity = 0f;
            playerControllerB.velocityLastFrame = Vector3.zero;
            StartOfRound.Instance.allPlayerScripts[playerObj].TeleportPlayer(teleportPos);
            StartOfRound.Instance.allPlayerScripts[playerObj].beamOutParticle.Play();
            shipTeleporterAudio.PlayOneShot(teleporterBeamUpSFX);
            StartOfRound.Instance.allPlayerScripts[playerObj].movementAudio.PlayOneShot(teleporterBeamUpSFX);
            if (playerControllerB == GameNetworkManager.Instance.localPlayerController) {

                Debug.Log("Teleporter shaking camera");
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
            }
            return false; // Cancel original code
        }

        // Just for the method to work
        private static void StartCoroutine(string routine) {}

        private static void SetPlayerTeleporterId(PlayerControllerB playerScript, int teleporterId) {}

        private static string teleportBodyOut(int playerObj, Vector3 teleportPosition) { return "Body Out"; }

        // Lower cooldown for inverse teleporter to 60sec
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void lowerCooldown(ref float ___cooldownAmount) { ___cooldownAmount = 60f; }
    }
}
