# Hearth
An adventure game in the style of The Legend of Zelda. You are in a winter wasteland area, and have a hearth that keeps you warm. If you leave the area that it warms, you start to lose health. But the fire is now dying. You can collect glowing "tinder" to feed the hearth, but it slowly becomes less and less effective. Eventually, you discover a new hearth, and may choose to feed it with tinder. This new hearth is not dying. If you continue feeding it, it grows so brilliantly that it melts the snow in the surrounding area, bringing back room for growth. This is your new hearth.

This game is being made for Global Game Jam '19, with the theme of: **What Home Means to You**
We're having fun playing with symbolism of change and moving to a new home.

# Work List:

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

## Music and Noises
### Music
Doing alright
* Player is near fire & the fire is ok
* Player is losing health / outside of warmth circle (but is still doing ok)

Not too hot
* Player is near fire and the fire is low (not really helping the player anymore, sorta sad, sorta scared)
* Player losing health / not safe (but is close to dying--not good, please help, but not blaring)

Discovered the hearth!
* Player discovers hearth
* Hearth grows brighter
* Hearth heals everything (finale)

### Noises
Environment - pretty consistent / repetitive
* It's snowing
* The fire is powerful
* The fire is dying (it never dies, but it does eventually stop being effective/supportive)

Action
* Player walking through snow
* Player walking through snow carrying tinder (heavy)
* Player picks up tinder
* Player puts tinder into the fire (3 versions of the same noise: ok, not ideal, & sorta pointless)
* Player puts tinder into hearth (3 versions: good, great, & you're healing the world and your inner being)
* Player dies
* Player dies after dropping the tinder they were carrying

## UI / UX
Start Menu
```
Background is the initial positioning of the player, fire, and piece of kindling, 
but with no health or strength bars. Small text in bottom right says "Press Spacebar"
```

Pause Menu
```
Overlay over screen (screen given a gray tint)
* Resume
* Quit
* Maybe some sorta impactful quote?
```

Death Menu
```
Overlay over screen (screen grays out to black)
* Restart
* Quit
* Maybe some sorta impactful quote?
```
