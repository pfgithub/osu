// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Framework.Bindables;
using osu.Game.Configuration;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModMinimumJudgement : Mod, IApplicableToHealthProcessor
    {
        public override string Name => "Minimum Judgement";
        public override string Acronym => "MJ";
        public override IconUsage? Icon => FontAwesome.Solid.ExclamationCircle;
        public override ModType Type => ModType.DifficultyIncrease;
        public override bool Ranked => true;
        public override bool RequiresConfiguration => true;
        public override double ScoreMultiplier => 1;
        public override string Description => "Anything below is a miss.";

        [SettingSource("Minimum Judgement", "Set the minimum judgement")]
        public Bindable<AllowedHitResult> MinimumJudgement { get; } = new Bindable<AllowedHitResult>(AllowedHitResult.Good);

        public enum AllowedHitResult
        {
            Ok,
            Good,
            Great,
            Perfect,
        }

        public void ApplyToHealthProcessor(HealthProcessor healthProcessor)
        {
            healthProcessor.BeforeJudgement += BeforeJudgement;
        }

        protected HitResult AllowedResultToHitResult(AllowedHitResult allowedResult)
        {
            switch (allowedResult)
            {
                case AllowedHitResult.Ok:
                    return HitResult.Ok;
                case AllowedHitResult.Good:
                    return HitResult.Good;
                case AllowedHitResult.Great:
                    return HitResult.Great;
                case AllowedHitResult.Perfect:
                    return HitResult.Perfect;
                default:
                    return HitResult.Miss;
            }
        }
        protected void BeforeJudgement(JudgementResult result)
        {
            if (result.Type.AffectsAccuracy()
            && result.Type < AllowedResultToHitResult(MinimumJudgement.Value))
            {
                result.Type = HitResult.Miss;
            }
        }
    }
}
