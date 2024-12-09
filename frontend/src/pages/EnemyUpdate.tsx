import { Spinner, useToast } from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Enemy, useEnemy } from "../hooks/useEnemy";
import EnemyUpsert from "../components/EnemyUpsert";


const EnemyUpdate = () => {
    const toast = useToast()
    const param = useParams().enemyId!;
    const value = Number(param);
    const [enemy, setEnemy] = useState<Enemy>();
    const { data, error } = useEnemy(value);
    useEffect(() => {
        if (data !== undefined) {
            setEnemy(data)
        }
    }, [data])
    useEffect(() => {
        if (error != null) {
            handleError();
        }
    }, [error])
    const handleError = () => {
        let message = "Something went wrong";
        if (error?.status == 401) {
            message = "You are not authorized to see the given page"
        }
        else if (error?.status == 404) {
            message = "Enemy not found"
        }
        toast({
            description: message,
            status: "error",
        });
    }
    if (enemy === undefined) {
        return <Spinner />
    }
    else {
        return <EnemyUpsert data={enemy} />
    }
}

export default EnemyUpdate;
