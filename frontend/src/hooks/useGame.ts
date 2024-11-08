import { useGet, usePost } from "./useData";
import { Enemy } from "./useEnemy";

export interface GameAction {
    type: string;
    value?: number;
}

interface player{
    id : number;
    health: number;
}


interface State {
    playerDTO : player;
    enemyDTO: Enemy;
}

export interface StateGameInit {
    playerId: number;
    enemyId: number;
}

// Initialize game state
const initGame = (stateGameInit: StateGameInit) => usePost<StateGameInit, State>(`/init-game-state` , stateGameInit);

// Fetch current game state
const gameState = () => useGet<State>(`/game-state`);

// Combat action hook
const useCombat = (action: GameAction) => {
    return usePost<GameAction, State>('/combat', action);
};

export { initGame, gameState, useCombat };
