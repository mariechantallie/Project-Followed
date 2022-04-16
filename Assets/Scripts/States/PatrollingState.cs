public class PatrollingState : MonoState {
    Ghoul ghoul;

    public PatrollingState(Ghoul ghoul, string name = "Patrolling") : base(name) {
        this.ghoul = ghoul;
    }

    public override void Enter() {
        ghoul.Patrol();
    }
}
