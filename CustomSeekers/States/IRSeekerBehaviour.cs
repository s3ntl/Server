using System;
using System.Reflection;
using ImprovedMissiles.Utils;
using UnityEngine;

namespace ImprovedMissiles.States
{
    public abstract class IRSeekerBehaviour
    {
        protected IRSeeker _irSeeker;

        private readonly object[] _emptyArgs = Array.Empty<object>();

        public IRSeekerBehaviour(IRSeeker irSeeker)
        {
            _irSeeker = irSeeker;
            InitializeFields();
            
        }

        // Чтение приватных полей IRSeeker с помощью Reflection
        protected T ReadPrivateField<T>(string fieldName)
        {
            FieldInfo fieldInfo = typeof(IRSeeker).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                throw new InvalidOperationException($"Field {fieldName} not found");
            return (T)fieldInfo.GetValue(_irSeeker);
        }

        protected void WritePrivateField<T>(string fieldName, T value)
        {
            FieldInfo fieldInfo = typeof(IRSeeker).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                throw new InvalidOperationException($"Field {fieldName} not found");
            fieldInfo.SetValue(_irSeeker, value);
        }

        // Создание делегата для вызова приватных методов IRSeeker
        protected Delegate CreatePrivateMethodDelegate(Type delegateType, string methodName)
        {
            MethodInfo methodInfo = typeof(IRSeeker).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} not found");
            return Delegate.CreateDelegate(delegateType, _irSeeker, methodInfo);
        }

        // Инициализация состояния
        public virtual void Initialize(Unit target, GlobalPosition aimPoint)
        {
            
        }

        public void InitializeFields()
        {
            // Примеры чтения приватных полей
            flareRejection = ReadPrivateField<float>("flareRejection");
            IRTarget = ReadPrivateField<IRSource>("IRTarget");
            knownPos = ReadPrivateField<GlobalPosition>("knownPos");
            knownVel = ReadPrivateField<Vector3>("knownVel");
            knownVelPrev = ReadPrivateField<Vector3>("knownVelPrev");
            knownAccel = ReadPrivateField<Vector3>("knownAccel");
            driftError = ReadPrivateField<Vector3>("driftError");
            errorOffset = ReadPrivateField<Vector3>("errorOffset");
            positionalError = ReadPrivateField<float>("positionalError");
            driftRate = ReadPrivateField<float>("driftRate");
            guidanceDelay = ReadPrivateField<float>("guidanceDelay");
            tangibleDelay = ReadPrivateField<float>("tangibleDelay");
            selfDestructAtSpeed = ReadPrivateField<float>("selfDestructAtSpeed");
            maxLead = ReadPrivateField<float>("maxLead");
            dazzleAmount = ReadPrivateField<float>("dazzleAmount");
            lastEvaluated = ReadPrivateField<float>("lastEvaluated");
            topSpeed = ReadPrivateField<float>("topSpeed");
            guidance = ReadPrivateField<bool>("guidance");
            achievedLock = ReadPrivateField<bool>("achievedLock");
            targetOnLaunch = ReadPrivateField<bool>("targetOnLaunch");
            missile = ReadPrivateField<Missile>("missile");
            targetUnit = ReadPrivateField<Unit>("targetUnit");
        }

        #region // Приватные поля класса митча 
        
        public float flareRejection;
        public IRSource IRTarget;
        public GlobalPosition knownPos;
        public Vector3 knownVel;
        public Vector3 knownVelPrev;
        public Vector3 knownAccel;
        public Vector3 driftError;
        public Vector3 errorOffset;
        public float positionalError;
        public float driftRate;
        public float guidanceDelay = 0.25f;
        public float tangibleDelay = 0.25f;
        public float selfDestructAtSpeed = 200f;
        public float maxLead = 5f;
        public float dazzleAmount;
        public float lastEvaluated;
        public float topSpeed;
        public bool guidance;
        public bool achievedLock;
        public bool targetOnLaunch;
        public Missile missile;
        public Unit targetUnit;
        #endregion

        public virtual void SlowChecks()
        {
            // Создаем делегат для вызова SlowChecks из IRSeeker
            var slowCheckDelegate = CreatePrivateMethodDelegate(typeof(Action), nameof(SlowChecks));
            slowCheckDelegate.DynamicInvoke(_emptyArgs); 
        }

        
        public virtual string GetSeekerType() => "";
        public virtual void Seek() { }
        public virtual void IRLockCheck() { }
        public virtual void IR_Seeker_OnTargetFlare(IRSource source) { }
        public virtual float AspectCoef(Vector3 targetVector) => 0f;
        public virtual float BackgroundBrightness(Vector3 targetVector) => 0f;
        public virtual void LoseLock() 
        {
            var loseLockDelegate = CreatePrivateMethodDelegate(typeof(Action), nameof(LoseLock));

            loseLockDelegate.DynamicInvoke();

           
        }
        public virtual void IRSeeker_OnMissileDestroyed(Unit unit) => LoseLock();
    }
}