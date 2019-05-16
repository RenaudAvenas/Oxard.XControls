using System;
using System.Collections.Generic;

namespace Oxard.XControls.Interpretors
{
    public static class InterpretorManager
    {
        private static readonly Dictionary<Type, IInterpretor> Interpretors = new Dictionary<Type, IInterpretor>();

        public static void Register(IInterpretor interpretor)
        {
            var type = interpretor.GetType();
            var interfaces = type.GetInterfaces();

            List<Type> matchingInterfaces = new List<Type>();
            Type interpretorInterface = typeof(IInterpretor);

            foreach (var @interface in interfaces)
            {
                if (@interface != interpretorInterface && interpretorInterface.IsAssignableFrom(@interface) && @interface.Namespace == interpretorInterface.Namespace)
                    matchingInterfaces.Add(@interface);
            }

            foreach (var matchingInterface in matchingInterfaces)
            {
                if (Interpretors.ContainsKey(matchingInterface))
                    throw new InvalidOperationException($"Type {type.FullName} already registred in interpretors");

                Interpretors[matchingInterface] = interpretor;
            }
        }

        public static void RegisterForType(Type type, IInterpretor interpretor)
        {
            if(Interpretors.ContainsKey(type))
                throw new InvalidOperationException($"Type {type.FullName} already registred in interpretors");

            Interpretors[type] = interpretor;
        }

        public static void RegisterForTypeIfNotExists(Type type, IInterpretor interpretor)
        {
            if (Interpretors.ContainsKey(type))
                return;

            Interpretors[type] = interpretor;
        }

        public static bool HasInterpretor<TInterpretor>() => Interpretors.ContainsKey(typeof(TInterpretor));

        public static bool HasInterpretorForType(Type type) => Interpretors.ContainsKey(type);

        public static IInterpretor GetForType(Type type)
        {
            if (!Interpretors.ContainsKey(type))
                return null;

            return Interpretors[type];
        }

        public static TInterpretor Get<TInterpretor>()
            where TInterpretor : class, IInterpretor
        {
            var type = typeof(TInterpretor);
            if (!Interpretors.ContainsKey(type))
                return null;

            return (TInterpretor)Interpretors[type];
        }
    }
}
