public class HuntingState : MonoState {
    Ghoul ghoul;

    public HuntingState(Ghoul ghoul, string name = "Hunting") : base(name) {
        this.ghoul = ghoul;
    }

    public override void Enter() {
        ghoul.Hunt();
    }
}
