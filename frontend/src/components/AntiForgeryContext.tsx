import React, { createContext, useContext, ReactNode } from "react";
import { fetchAntiForgeryToken } from "../hooks/useAntiForgery";

interface AntiForgeryContextType {
  initializeAntiForgery: () => Promise<void>;
}

const AntiForgeryContext = createContext<AntiForgeryContextType | undefined>(
  undefined
);

export const AntiForgeryProvider = ({ children }: { children: ReactNode }) => {
  const initializeAntiForgery = async () => {
    await fetchAntiForgeryToken();
  };

  return (
    <AntiForgeryContext.Provider value={{ initializeAntiForgery }}>
      {children}
    </AntiForgeryContext.Provider>
  );
};

export const useAntiForgery = (): AntiForgeryContextType => {
  const context = useContext(AntiForgeryContext);
  if (!context) {
    throw new Error("useAntiForgery must be used within an AntiForgeryProvider");
  }
  return context;
};
