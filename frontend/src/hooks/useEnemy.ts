import { useGet, usePost, usePut } from "./useData";

export interface Enemy {
    id?: number;
    name: string;
    health: number;
    cost: number;
    imagePath: string;
}

const useEnemies = () => useGet<Enemy[]>(`/enemies`)
const useEnemy = (id: number) => useGet<Enemy>(`/enemies/${id}`)
const usePostEnemy = (enemy: Enemy) => usePost<Enemy, Enemy>(`/enemies`,enemy);
const usePutEnemy = (id: number,enemy: Enemy) => usePut<Enemy, Enemy>(`/enemies/${id}`,enemy); 

export { useEnemy, useEnemies, usePostEnemy,usePutEnemy };