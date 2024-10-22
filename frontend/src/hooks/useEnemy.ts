import { useGet, usePost, usePut } from "./useData";

export interface Enemy {
    id?: number;
    name: string;
    health: string;
    cost: number;
    imagePath: string;
}

const useEnemy = () => useGet<Enemy[]>(`/enemies`)
const useEnemies = (id: number) => useGet<Enemy>(`/enemies/${id}`)
const usePostEnemy = () => usePost<Enemy, Enemy>(`/enemies`);
const usePutEnemy = (id: number) => usePut<Enemy, Enemy>(`/enemies/${id}`); 

export { useEnemy, useEnemies, usePostEnemy,usePutEnemy };