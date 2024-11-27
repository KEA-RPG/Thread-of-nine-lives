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

const initGame = (stateGameInit: StateGameInit) => usePost<StateGameInit, State>(`/combat/initialize` , stateGameInit);
const useGameState = (fightId: number) => useGet<State>(`/combat/${fightId}`);
const useCombat = (fightId: number, action: GameAction) => usePost<GameAction, State>(`/combat/${fightId}/action`, action);

export { initGame, useCombat, useGameState };
