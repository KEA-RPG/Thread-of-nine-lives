import { useGet, usePost } from "./useData";
import { Enemy } from "./useEnemy";

export interface GameAction {
    type: string;
    value?: number;
}

export interface State {
    FightId: number;
    PlayerHealth: number;
    EnemyHealth: number;
}

export interface StateGameInit {
    enemyId: number;
}

// Initialize game state
const initGame = (stateGameInit: StateGameInit) => usePost<StateGameInit, State>(`/init-game-state` , stateGameInit);

// Combat action hook
const useCombat = (action: GameAction) => {
    return usePost<GameAction, State>('/combat', action);
};

export { initGame, useCombat };
