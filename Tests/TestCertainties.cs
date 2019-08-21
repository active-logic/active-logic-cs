using NUnit.Framework;

using Active.Core;
using InvOp = System.InvalidOperationException;

public class TestCertainties{

public class TestAction : CoreTest{

    [Test] public void Action()       => o ( action._void.now.complete );

    [Test] public void InvertAction() => o( !action._void, failure._flop );

    [Test] public void CombineActions()
    { action x = action._void%action._void; }

    #if !AL_STRICT
    [Test] public void ToStatus(){
        status s = action._void;
        o( s, status._done );
    }
    #endif

}

public class TestFailure : CoreTest{

    [Test] public void Failure() => o ( failure._flop.fail.failing  );
    [Test] public void InvertFailure() => o( !failure._flop, action._void );
    [Test] public void CombineFailures()
    { failure x = failure._flop % failure._flop; }

    #if !AL_STRICT
    [Test] public void ToStatus(){
        status s = failure._flop;
        o( s, status._fail );
    }
    #endif

}

public class TestLoop : CoreTest{

    [Test] public void Loop() => o ( loop._forever.ever.running  );
    [Test] public void CombineLoops() { loop x = loop._forever%loop._forever; }

    #if !AL_STRICT
    [Test] public void ToStatus(){
        status s = loop._forever;
        o( s, status._cont );
    }
    #endif

}

public class TestPending : CoreTest{

    [Test] public void PendingDone()     => o ( pending.done().due.complete);
    [Test] public void PendingCont()     => o ( pending.cont().due.running);
    [Test] public void Pending_Done()    => o ( pending._done.due.complete);
    [Test] public void Pending_Cont()    => o ( pending._cont.due.running);
    [Test] public void InvPending_Done() => o (!pending._done, impending._doom);
    [Test] public void InvPending_Cont() => o (!pending._cont, impending._cont);

    [Test] public void AndPending(){
        o(pending._cont && pending._cont, pending._cont);
        o(pending._cont && pending._done, pending._cont);
        o(pending._done && pending._cont, pending._cont);
        o(pending._done && pending._done, pending._done);
    }

    [Test] public void OrPending([Values(0, +1)] int a, [Values(0, +1)] int b)
    => Assert.Throws<InvOp>( () =>
    { var s = new pending(a) || new pending(b); } );

    #if !AL_STRICT
    [Test] public void ToStatus(){
        status s = pending._cont;
        o( s, status._cont );
    }
    #endif

}

public class TestImpending : CoreTest{

    [Test] public void ImpendingDoom()   => o( impending.doom().undue.failing );
    [Test] public void ImpendingCont()   => o( impending.cont().undue.running );
    [Test] public void Impending_Doom()   => o( impending._doom.undue.failing );
    [Test] public void Impending_Cont()   => o( impending._cont.undue.running );
    [Test] public void InvImpendingDoom() => o(!impending._doom, pending._done);
    [Test] public void InvImpendingCont() => o(!impending._cont, pending._cont);

    [Test] public void AndImpending([Values(-1, 0)]int a, [Values(-1, 0)]int b)
    => Assert.Throws<InvOp>( () =>
        { var s = new impending(a) && new impending(b); } );
    [Test] public void OrImpending(){
        o(impending._cont || impending._cont, impending._cont);
        o(impending._cont || impending._doom, impending._cont);
        o(impending._doom || impending._cont, impending._cont);
        o(impending._doom || impending._doom, impending._doom);
    }

    #if !AL_STRICT
    [Test] public void ToStatus(){
        status s = impending._cont;
        o( s, status._cont );
    }
    #endif

}

}
