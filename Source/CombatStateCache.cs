using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CantYouSeeImBusy
{
    public class CombatStateCache : MapComponent
    {
        public bool InCombat { get; private set; }

        private Dictionary<int, int> _gracePeriodStartTick = new Dictionary<int, int>();
        private const int GracePeriodTicks = 125;

        private Dictionary<int, int> _lastLetterTick = new Dictionary<int, int>();
        private const int LetterThrottleTicks = 500;

        public CombatStateCache(Map map) : base(map) { }

        public override void MapComponentTick()
        {
            InCombat = GenHostility.AnyHostileActiveThreatToPlayer(map);
        }

        public static CombatStateCache? GetFor(Map map)
        {
            return map?.GetComponent<CombatStateCache>();
        }

        public static bool IsMapInCombat(Map map)
        {
            return GetFor(map)?.InCombat ?? false;
        }

        public void StartGracePeriod(Pawn pawn)
        {
            if (pawn == null) return;
            _gracePeriodStartTick[pawn.thingIDNumber] = Find.TickManager.TicksGame;
        }

        public void ClearGracePeriod(Pawn pawn)
        {
            if (pawn == null) return;
            _gracePeriodStartTick.Remove(pawn.thingIDNumber);
        }

        public bool IsInGracePeriod(Pawn pawn)
        {
            if (pawn == null || pawn.Dead || !pawn.Spawned) return false;
            if (!_gracePeriodStartTick.TryGetValue(pawn.thingIDNumber, out int startTick)) return false;
            return (Find.TickManager.TicksGame - startTick) < GracePeriodTicks;
        }

        public bool ShouldSendLetter(Pawn pawn)
        {
            if (pawn == null) return false;
            int now = Find.TickManager.TicksGame;
            if (_lastLetterTick.TryGetValue(pawn.thingIDNumber, out int lastTick) && (now - lastTick) < LetterThrottleTicks)
                return false;
            _lastLetterTick[pawn.thingIDNumber] = now;
            return true;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref _gracePeriodStartTick, "gracePeriodStartTick", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref _lastLetterTick, "lastLetterTick", LookMode.Value, LookMode.Value);
            _gracePeriodStartTick ??= new Dictionary<int, int>();
            _lastLetterTick ??= new Dictionary<int, int>();
        }
    }
}
