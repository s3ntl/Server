using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ServerTools.Upgrades
{
    public static class MissileHelper
    {
        // motor cache fields
        private static readonly Type MotorType;
        private static readonly FieldInfo ThrustField;
        private static readonly FieldInfo BurnTimeField;
        private static readonly FieldInfo FuelMassField;
        private static readonly FieldInfo TopSpeedField;
        private static readonly FieldInfo ThrustVectoringField;
        private static readonly FieldInfo DelayTimerField;
        private static readonly FieldInfo IrIntensityField;
        private static readonly FieldInfo MotorsField;

        // missile cache fields
        private static readonly FieldInfo MassField;
        private static readonly FieldInfo FinAreaField;
        private static readonly FieldInfo TorqueField;
        private static readonly FieldInfo GLimitField;
        private static readonly FieldInfo SupersonicDragField;
        private static readonly FieldInfo LiftCurveField;
        private static readonly FieldInfo DragCurveField;
        private static readonly FieldInfo BlastYieldField;
        private static readonly FieldInfo PierceDamageField;
        private static readonly FieldInfo ImpactFuseDelayField;
        private static readonly FieldInfo ProximityFuseField;
        private static readonly FieldInfo PidFactorsField;
        private static readonly FieldInfo UprightPreferenceField;

        // setting field info
        static MissileHelper()
        {
            Type missileType = typeof(Missile);

            
            MotorType = missileType.GetNestedType("Motor", BindingFlags.NonPublic);
            if (MotorType == null)
                throw new InvalidOperationException("no motor class");

            ThrustField = MotorType.GetField("thrust");
            BurnTimeField = MotorType.GetField("burnTime");
            FuelMassField = MotorType.GetField("fuelMass");
            TopSpeedField = MotorType.GetField("topSpeed");
            ThrustVectoringField = MotorType.GetField("thrustVectoring");
            DelayTimerField = MotorType.GetField("delayTimer");
            IrIntensityField = MotorType.GetField("IR_intensity");

            MotorsField = missileType.GetField("motors", BindingFlags.NonPublic | BindingFlags.Instance);

            
            MassField = missileType.GetField("mass", BindingFlags.NonPublic | BindingFlags.Instance);
            FinAreaField = missileType.GetField("finArea", BindingFlags.NonPublic | BindingFlags.Instance);
            TorqueField = missileType.GetField("torque", BindingFlags.NonPublic | BindingFlags.Instance);
            GLimitField = missileType.GetField("gLimit", BindingFlags.NonPublic | BindingFlags.Instance);
            SupersonicDragField = missileType.GetField("supersonicDrag", BindingFlags.NonPublic | BindingFlags.Instance);
            LiftCurveField = missileType.GetField("liftCurve", BindingFlags.NonPublic | BindingFlags.Instance);
            DragCurveField = missileType.GetField("dragCurve", BindingFlags.NonPublic | BindingFlags.Instance);
            BlastYieldField = missileType.GetField("blastYield", BindingFlags.NonPublic | BindingFlags.Instance);
            PierceDamageField = missileType.GetField("pierceDamage", BindingFlags.NonPublic | BindingFlags.Instance);
            ImpactFuseDelayField = missileType.GetField("impactFuseDelay", BindingFlags.NonPublic | BindingFlags.Instance);
            ProximityFuseField = missileType.GetField("proximityFuse", BindingFlags.NonPublic | BindingFlags.Instance);
            PidFactorsField = missileType.GetField("PIDFactors", BindingFlags.NonPublic | BindingFlags.Instance);
            UprightPreferenceField = missileType.GetField("uprightPreference", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        
        public static object[] GetMotors(this Missile missile) => (object[])MotorsField.GetValue(missile);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static float GetThrust(object motor) => (float)ThrustField.GetValue(motor);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetThrust(object motor, float value) => ThrustField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetBurnTime(object motor, float value) => BurnTimeField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetFuelMass(object motor, float value) => FuelMassField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetTopSpeed(object motor, float value) => TopSpeedField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor type
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetThrustVectoring(object motor, float value) => ThrustVectoringField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor type 
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetDelayTimer(object motor, float value) => DelayTimerField.SetValue(motor, value);
        /// <summary>
        /// ONLY FOR Missile.Motor
        /// </summary>
        /// <param name="motor"></param>
        /// <returns></returns>
        public static void SetIRIntensity(object motor, float value) => IrIntensityField.SetValue(motor, value);

        // missile methods
        public static float GetMass(this Missile missile) => (float)MassField.GetValue(missile);
        public static void SetMass(this Missile missile, float value) => MassField.SetValue(missile, value);

        public static float GetFinArea(this Missile missile) => (float)FinAreaField.GetValue(missile);
        public static void SetFinArea(this Missile missile, float value) => FinAreaField.SetValue(missile, value);

        public static float GetTorque(this Missile missile) => (float)TorqueField.GetValue(missile);
        public static void SetTorque(this Missile missile, float value) => TorqueField.SetValue(missile, value);

        public static float GetGLimit(this Missile missile) => (float)GLimitField.GetValue(missile);
        public static void SetGLimit(this Missile missile, float value) => GLimitField.SetValue(missile, value);

        public static float GetSupersonicDrag(this Missile missile) => (float)SupersonicDragField.GetValue(missile);
        public static void SetSupersonicDrag(this Missile missile, float value) => SupersonicDragField.SetValue(missile, value);

        public static AnimationCurve GetLiftCurve(this Missile missile) => (AnimationCurve)LiftCurveField.GetValue(missile);
        public static void SetLiftCurve(this Missile missile, AnimationCurve value) => LiftCurveField.SetValue(missile, value);

        public static AnimationCurve GetDragCurve(this Missile missile) => (AnimationCurve)DragCurveField.GetValue(missile);
        public static void SetDragCurve(this Missile missile, AnimationCurve value) => DragCurveField.SetValue(missile, value);

        public static float GetBlastYield(this Missile missile) => (float)BlastYieldField.GetValue(missile);
        public static void SetBlastYield(this Missile missile, float value) => BlastYieldField.SetValue(missile, value);

        public static float GetPierceDamage(this Missile missile) => (float)PierceDamageField.GetValue(missile);
        public static void SetPierceDamage(this Missile missile, float value) => PierceDamageField.SetValue(missile, value);

        public static float GetImpactFuseDelay(this Missile missile) => (float)ImpactFuseDelayField.GetValue(missile);
        public static void SetImpactFuseDelay(this Missile missile, float value) => ImpactFuseDelayField.SetValue(missile, value);

        public static bool GetProximityFuse(this Missile missile) => (bool)ProximityFuseField.GetValue(missile);
        public static void SetProximityFuse(this Missile missile, bool value) => ProximityFuseField.SetValue(missile, value);

        public static float GetUprightPreference(this Missile missile) => (float)UprightPreferenceField.GetValue(missile);
        public static void SetUprightPreference(this Missile missile, float value) => UprightPreferenceField.SetValue(missile, value);

        // pid
        public static object GetPIDFactors(this Missile missile) => PidFactorsField.GetValue(missile);
        public static void SetPIDFactors(this Missile missile, object value) => PidFactorsField.SetValue(missile, value);

        public static void ModifyPIDFactors(this Missile missile, Func<float, float> pMod = null, Func<float, float> iMod = null, Func<float, float> dMod = null)
        {
            object pidObj = missile.GetPIDFactors();
            if (pidObj == null) return;

            Type pidType = pidObj.GetType();
            FieldInfo pidVectorField = pidType.GetField("PID", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pidVectorField == null) return;

            UnityEngine.Vector3 pidVector = (UnityEngine.Vector3)pidVectorField.GetValue(pidObj);
            if (pMod != null) pidVector.x = pMod(pidVector.x);
            if (iMod != null) pidVector.y = iMod(pidVector.y);
            if (dMod != null) pidVector.z = dMod(pidVector.z);
            pidVectorField.SetValue(pidObj, pidVector);
        }

        //rb sync
        public static void SyncMassToRigidbody(this Missile missile)
        {
            var rb = missile.GetComponent<UnityEngine.Rigidbody>();
            if (rb != null)
                rb.mass = missile.GetMass();
        }
    }
}