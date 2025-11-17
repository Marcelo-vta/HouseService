using UnityEngine;

/// <summary>
/// This script is now part of the centralized enemy system.
/// It inherits from the base Enemy class to gain access to EnemyStats.
/// The actual movement and AI logic has been moved to GhostAI.cs for better separation of concerns.
/// </summary>
public class GhostMovement : Enemy
{
    // Intentionally left blank.
    // This class's purpose is to connect the Ghost to the Enemy system
    // and provide a component for the GhostAI script to access stats from.
}
