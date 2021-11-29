﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;


public class FSM {

	private Stack<FSMState> stateStack = new Stack<FSMState> ();

	public delegate void FSMState (FSM fsm, GameObject gameObject);
	

	public void Update (GameObject gameObject) {
		var state = stateStack.Peek();
		state?.Invoke (this, gameObject);
	}

	public void pushState(FSMState state) {
		stateStack.Push (state);
	}

	public void popState() {
		stateStack.Pop ();
	}
}
