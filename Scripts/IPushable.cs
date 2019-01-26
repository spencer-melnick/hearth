using Godot;
using System;

namespace AdventureGame {
    public interface IPushable
    {
        void TryPush(Vector2 normal, float delta);
    }
}