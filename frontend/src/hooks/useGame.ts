import { useGet, usePost } from "./useData";

export interface GameAction {
    type: string;
    value?: number;
}

export interface State {
    fightId: number;
    playerHealth: number;
    enemyHealth: number;
}

export interface StateGameInit {
    enemyId: number;
}

// Initialize game state
const initGame = (stateGameInit: StateGameInit) => usePost<StateGameInit, State>(`/combat/initialize` , stateGameInit);

// Combat action hook
const useCombat = (fightId: number, action: GameAction) => {
    return usePost<GameAction, State>(`/combat/${fightId}/action`, action);
};

// Get game state
const useGameState = (fightId: number) => useGet<State>(`/combat/${fightId}`);

export { initGame, useCombat, useGameState };
