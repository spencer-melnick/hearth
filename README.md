# Hearth
A quick prototype of a Link to the Past inspired Adventure/Role Playing Game in the Godot engine.

# TODO:

## Coding

### Basic Elements
Fire (old home)
* Has a Strength bar
* Fire strength slowly drains
* Fire radius ** Note: Radius of campfire never changes **

Player
* Exists and can move/collide
* Has a Health bar
* Health drains when outside campfire radius
* Health regained when inside campfire radius

Kindling
* Exists in world
* Can be picked up
* Can be added to fire
* When added to fire, fire health goes up

Hearth (new home)
* exists
* when player can "see" it, new strength bar appears
* when kindling added, 1/5 hearth strength bar filled


### Game Design-y bits
Campfire
* First time kindling added, full strength + trigger animation for "yay!"
* Second time kindling added, slightly less strength added + trigger animation for "meh"
* we can decide ramp up, but the fire heals less each kindling
* As fire is lower strength, it heals player less
* Fire has a minimum strength, but it heals the player less than the player loses health

Hearth
* heals the player according to level of fire health (slower if lower, faster if higher)

Design Map
* Place kindling in slightly further away from fire locations
* one kindling piece right next to the hearth

## Art Assets
Player
* Facing down
* Facing up
* Facing left / right (left and right could just be mirrors)
* Holding kindling up
* Holding kindling down
* Holding kindling left / right

Fire

Kindling
* maybe a couple alternate options (but similar enough to look the same when picked up)

Hearth
* Before adding the first kindling (just a base)
* After adding kindling (crystal appears)

Scenery
* Trees
* Rocks
* Whatever else we feel

## Animation
Player
* Walking
* Walking with kindling
* Picking up kindling
* Adding kindling to fire
* Lower health --> maybe add a blue shader?

Fire
* As fire gets weaker, apply a shader of some sort
* Maybe "strength" light/rings that show the current power of the fire (also decreases with strength)

Kindling
* Picked up

Hearth
* First kindling added --> crystal appears
* Gradually increasing in strength (similar to the fire but generally in reverse)

Environment / Screen
* Snow?
* Player death
* Final piece of kindling added to hearth --> snow melting around the area & spring and joy returning!


