using System;
using System.Reflection;
using HarmonyLib; 

namespace ServerTools.Upgrades
    {
    public static class MissileMotorReflection
    {
        
        private static readonly Type MotorType;

        
        private static readonly FieldInfo ThrustField;
        private static readonly FieldInfo BurnTimeField;
        private static readonly FieldInfo FuelMassField;
        private static readonly FieldInfo TopSpeedField;
        private static readonly FieldInfo ThrustVectoringField;
        private static readonly FieldInfo DelayTimerField;
        private static readonly FieldInfo IrIntensityField;

        
        private static readonly FieldInfo MotorsField;

        static MissileMotorReflection()
        {
           
            Type missileType = typeof(Missile); 

            
            MotorType = missileType.GetNestedType("Motor", BindingFlags.NonPublic);
            if (MotorType == null)
                throw new InvalidOperationException("Тип Missile.Motor не найден");

            
            ThrustField = MotorType.GetField("thrust");
            BurnTimeField = MotorType.GetField("burnTime");
            FuelMassField = MotorType.GetField("fuelMass");
            TopSpeedField = MotorType.GetField("topSpeed");
            ThrustVectoringField = MotorType.GetField("thrustVectoring");
            DelayTimerField = MotorType.GetField("delayTimer");
            IrIntensityField = MotorType.GetField("IR_intensity");

            
            MotorsField = missileType.GetField("motors", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        
        public static object[] GetMotors(Missile missile)
        {
            return (object[])MotorsField.GetValue(missile);
        }

        
        public static void SetThrust(object motor, float value)
        {
            ThrustField.SetValue(motor, value);
        }

       
        public static float GetThrust(object motor)
        {
            return (float)ThrustField.GetValue(motor);
        }

        
        public static void SetBurnTime(object motor, float value) => BurnTimeField.SetValue(motor, value);
        public static void SetFuelMass(object motor, float value) => FuelMassField.SetValue(motor, value);
        public static void SetTopSpeed(object motor, float value) => TopSpeedField.SetValue(motor, value);
        public static void SetThrustVectoring(object motor, float value) => ThrustVectoringField.SetValue(motor, value);
        public static void SetDelayTimer(object motor, float value) => DelayTimerField.SetValue(motor, value);
        public static void SetIRIntensity(object motor, float value) => IrIntensityField.SetValue(motor, value);
    }
}