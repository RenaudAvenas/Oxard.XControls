using Oxard.XControls.Graphics;
using System;
using System.Collections.Generic;

namespace Oxard.XControls.Interpretors
{
    /// <summary>
    /// Manage and register interpretors. Interpretors can be used to define how to manage a feature that does not enable on each platform to tranform the orginal feature in a replacement feature.
    /// </summary>
    /// <example><see cref="RadialGradientBrush"/> is not supported in UWP but can be transform to a linear gradient brush by an interpretor</example>
    public static class InterpretorManager
    {
        private static readonly Dictionary<Type, IInterpretor> Interpretors = new Dictionary<Type, IInterpretor>();

        /// <summary>
        /// Register an interpretor
        /// </summary>
        /// <param name="interpretor">Interpretor to register with <see cref="IInterpretor"/> key and instance for value</param>
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

        /// <summary>
        /// Register an interpretor for a specific <paramref name="type"/>
        /// </summary>
        /// <param name="type">Key type for registering</param>
        /// <param name="interpretor">Interpretor to register with <paramref name="type"/> key and instance for value</param>
        public static void RegisterForType(Type type, IInterpretor interpretor)
        {
            if(Interpretors.ContainsKey(type))
                throw new InvalidOperationException($"Type {type.FullName} already registred in interpretors");

            Interpretors[type] = interpretor;
        }

        /// <summary>
        /// Register an interpretor for a specific <paramref name="type"/> if not already exists
        /// </summary>
        /// <param name="type">Key type for registering</param>
        /// <param name="interpretor">Interpretor to register with <paramref name="type"/> key and instance for value</param>
        public static void RegisterForTypeIfNotExists(Type type, IInterpretor interpretor)
        {
            if (Interpretors.ContainsKey(type))
                return;

            Interpretors[type] = interpretor;
        }

        /// <summary>
        /// Check if an interpretor of type <typeparamref name="TInterpretor"/> is registered
        /// </summary>
        /// <typeparam name="TInterpretor">Key type of interpretor to search</typeparam>
        /// <returns>True if interpretor already registered</returns>
        public static bool HasInterpretor<TInterpretor>() => Interpretors.ContainsKey(typeof(TInterpretor));

        /// <summary>
        /// Check if an interpretor of type <paramref name="type"/> is registered
        /// </summary>
        /// <param name="type">Key type of interpretor to search</param>
        /// <returns>True if interpretor already registered</returns>
        public static bool HasInterpretorForType(Type type) => Interpretors.ContainsKey(type);

        /// <summary>
        /// Return the interpretor to use for this <paramref name="type"/>
        /// </summary>
        /// <param name="type">Key type</param>
        /// <returns>Interpretor if key type exists otherwise null</returns>
        public static IInterpretor GetForType(Type type)
        {
            if (!Interpretors.ContainsKey(type))
                return null;

            return Interpretors[type];
        }

        /// <summary>
        /// Return the interpretor to use for this <typeparamref name="TInterpretor"/>
        /// </summary>
        /// <typeparam name="TInterpretor">Key type of interpretor to search</typeparam>
        /// <returns>Interpretor if key type exists otherwise null</returns>
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
