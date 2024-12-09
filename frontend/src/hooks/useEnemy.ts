import { ListItemBase, useDelete, useGet, usePost, usePut } from "./useData";

export interface Enemy extends ListItemBase {
    id?: number;
    name: string;
    health: number;
    imagePath: string;
}

const useEnemies = () => useGet<Enemy[]>(`/enemies`)
const useEnemy = (id: number) => useGet<Enemy>(`/enemies/${id}`)
const postEnemy = (enemy: Enemy) => usePost<Enemy, Enemy>(`/enemies`, enemy);
const putEnemy = (id: number, enemy: Enemy) => usePut<Enemy, Enemy>(`/enemies/${id}`, enemy);
const deleteEnemy = (id: number) => useDelete<Enemy>(`/enemies/${id}`);
export { useEnemy, useEnemies, postEnemy, putEnemy,deleteEnemy };