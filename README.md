# DoQuest

A gamified task/habit tracker written in C#. TUI (Terminal User Interface) via [Terminal.Gui][1], and persistence by [SQLite][2].

## What does this do?

The current goal is to provide a quick and easy interface for logging a task with an arbitrary “score”, like this:

`Do mobility workout    10`

The app then keeps track of your cumulative score, and awards you new “levels” as you hit certain score thresholds, as though this were a video game ([this one, specifically][3] 🐉⚔️).

### What will it eventually do?

*(Boring grownups call this section the “Roadmap”)*

I’d like to add...

- [ ] Treasure: A set of (procedurally-generated, probably) items that you’ll get rewarded for entering tasks, in addition to points, e.g. “Unusual Ruby of Wizard’s Artifice” (yes, I played a lot of Diablo back in the day, why do you ask? 👹)
  - ⚙️ Work in progress!
- [ ] Character “Classes”: I’ve always liked the mechanic (in games like Pokemon or Ogre Battle) where your character can “evolve” into a more interesting character class as they progress, e.g. “Adventurer” to “Wizard” to “Fire Magus”.
  - [ ] Figure out whether classes should be pickable on level up, or just assigned randomly (taking into account factors like character level and loot).
- [ ] Support for multiple “characters”.
  - [ ] The simple way: when you get your “character” to, say, Level 100, the app just starts a new one for you, and *that* character gets the points from now on.
      - ⚙️ I’ve built the foundation for this (having multiple characters with one flagged as “current”) but no logic yet for handling “retirement”.
  - [ ] The more complicated way: The app can keep track of the “progress” of multiple “characters” at a time, and let you switch between them when you add a task, if you want to.
- [ ] “Quests”, i.e. tasks that the app will keep track of for you, and then you can log that you’ve completed them, or perhaps completed part of them.
- [ ] History/Milestone log: A view that shows what tasks you entered on each day, and also when you hit certain “milestones”, like `Reached Level 5!` or `Got Rare Diamond Axe!`
- [ ] Deployment: A person who isn’t me should be able to easily download, install, and run this thing.

## Why would you want to do that?

I don’t know about you, but my neurodivergent brain does not like simply “doing things” it’s “supposed to” be “doing”. 

Even if I genuinely *want* to do those things!

I’ve played around with various ways of conditioning myself into habits—peanut butter M&Ms[^1] are a pretty good tactic—and this is my latest: giving myself “points” for completing regular and irregular tasks might encourage me to do so more often.

## Aren’t there already lots of apps that do this and have additional advantages, like they run on your phone?

Yes, but: 

1. Most of them have obnoxious payment dynamics, as the economics of the app stores unfortunately demand.
2. More importantly, I’m a software developer and sometimes you just want to develop your own software... else what is life for?

[1]: https://github.com/gui-cs/Terminal.Gui
[2]: https://sqlite.org
[3]: http://shrines.rpgclassics.com/nes/dw1/

[^1]: Not *peanut* M&Ms, *peanut butter* M&Ms, the ones in the orange bag. They’re my favourite. I eat them like *candy*.