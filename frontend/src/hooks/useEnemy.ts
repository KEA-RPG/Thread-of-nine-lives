import { useGet, usePost, usePut } from "./useData";

export interface Enemy {
    id?: number;
    name: string;
    health: string;
    cost: number;
    imagePath: string;
}

const useEnemies = () => useGet<Enemy[]>(`/enemies`)
const useEnemy = (id: number) => useGet<Enemy>(`/enemies/${id}`)
const postEnemy = (enemy: Enemy) => usePost<Enemy, Enemy>(`/enemies`, enemy);
const putEnemy = (id: number, enemy: Enemy) => usePut<Enemy, Enemy>(`/enemies/${id}`, enemy);

export { useEnemy, useEnemies, postEnemy, putEnemy };