using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeaderState {
    void UpdateState();

    void ToSentryState();

    void ToSearchState();

    void ToShootState();

    void ToFleeState();

}
